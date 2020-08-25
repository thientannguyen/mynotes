import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';

export default class NotificationService {
    private url: string;
    private connection: HubConnection | null;
    private entityHandlers: Map<
        string,
        Array<(entityId: string) => Promise<void>>
    >;
    private events: Map<string, boolean>;
    private static timerIds: NodeJS.Timeout[] = [];

    constructor(url: string) {
        this.url = url;
        this.connection = null;
        this.entityHandlers = new Map<
            string,
            Array<(entityId: string) => Promise<void>>
        >();
        this.onEntityChanged = this.onEntityChanged.bind(this);
        this.publishEvents = this.publishEvents.bind(this);
        this.events = new Map<string, boolean>();
        const timerId = setInterval(this.publishEvents, 100);
        NotificationService.timerIds.push(timerId);
    }

    public static shutdownTimers() {
        for (const timerId of NotificationService.timerIds) {
            clearInterval(timerId);
        }
    }

    private async registerEntity(
        entityId: string,
        onEntityChanged: (entityId: string) => {}
    ): Promise<void> {
        if (this.connection == null) {
            this.connection = await this.Connect();
            this.connection.on('NotifyChange', onEntityChanged);
        }
        try {
            await this.connection.invoke('Register', entityId);
        } catch (error) {
            this.OnConnectionFailed(error, this.connection!);
        }
    }

    private async unRegisterEntity(
        entityId: string,
        onEntityChanged: (entityId: string) => {}
    ): Promise<void> {
        if (this.connection == null) {
            return;
        }
        try {
            await this.connection.invoke('Unregister', entityId);
        } catch (error) {
            this.OnConnectionFailed(error, this.connection);
        }
    }

    private async Connect(): Promise<HubConnection> {
        const connection = new HubConnectionBuilder().withUrl(this.url).build();
        await connection.start();
        connection.onclose((error: Error | undefined) =>
            this.OnConnectionFailed(error, connection)
        );
        return connection;
    }

    private OnConnectionFailed(
        error: Error | undefined,
        connection: HubConnection
    ) {
        console.error(
            `NotificationHub failed signalR connection to ${this.url} Error: ${error}`
        );
        if (this.connection === connection) {
            this.connection = null;
        }
    }

    public async register(
        entityId: string,
        handler: (entityId: string) => Promise<void>
    ): Promise<void> {
        let handlers = this.entityHandlers.get(entityId);
        let registrationRequired = false;

        if (handlers === undefined) {
            handlers = [];
            registrationRequired = true;
        }

        const index = handlers.indexOf(handler, 0);
        if (index > -1) {
            handlers[index] = handler;
        } else {
            handlers.push(handler);
        }
        this.entityHandlers.set(entityId, handlers);

        if (registrationRequired) {
            await this.registerEntity(entityId, this.onEntityChanged);
        }
    }

    public async unRegister(
        entityId: string,
        handler: (entityId: string) => Promise<void>
    ): Promise<void> {
        let handlers = this.entityHandlers.get(entityId);
        if (handlers === undefined) {
            return;
        }

        if (!handlers.includes(handler)) {
            return;
        }

        // idea: new handlers array with array.filter()
        const index = handlers.indexOf(handler, 0);
        if (index > -1) {
            handlers.splice(index, 1);
        }

        if (handlers.length === 0) {
            this.entityHandlers.delete(entityId);
            await this.unRegisterEntity(entityId, this.onEntityChanged);
        }
    }

    private async publishEvents() {
        if (this.events.size > 0) {
            const currentEvents = this.events;
            this.events = new Map<string, boolean>();
            let handlePromises: Promise<void>[] = [];
            currentEvents.forEach((value: boolean, key: string) => {
                let handlers = this.entityHandlers.get(key);
                if (handlers !== undefined) {
                    handlePromises = handlePromises.concat(
                        handlers.map(async (handler) => handler(key))
                    );
                }
            });
            await Promise.all(handlePromises);
        }
    }

    private async onEntityChanged(entityId: string): Promise<void> {
        let handlers = this.entityHandlers.get(entityId);
        if (handlers === undefined) {
            return;
        }

        let event = this.events.get(entityId);
        if (event !== undefined) {
            return;
        }

        this.events.set(entityId, true);
    }
}
