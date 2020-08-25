import { Note } from './Note';

export class User {
    public id: string;
    public name: string;
    public notes: Note[];

    constructor(id: string, name: string, notes: Note[]) {
        this.id = id;
        this.name = name;
        this.notes = notes;
    }
}
