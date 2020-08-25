import { makeStyles } from '@material-ui/core/styles';

const useGlobalStyles = makeStyles((theme: any) => ({
    root: {
        display: 'flex',
        fallbacks: [
            { display: '-webkit-box' },
            { display: '-moz-box' },
            { display: '-ms-flexbox' },
            { display: '-webkit-flex' },
        ],
    },
    baseFrameContainer: {
        // iOS https://css-tricks.com/css-fix-for-100vh-in-mobile-webkit/
        fallbacks: [
            {
                minHeight: '-webkit-fill-available',
                height: '100vh',
            },
        ],
        width: '100vw',
        height: 'calc(100 * var(--vh))',
        maxWidth: '800px',
        maxHeight: '1000px',
        display: 'flex',
        justifyContent: 'center',
        flexDirection: 'column',
        backgroundColor: '#FFFF',
    },

    primaryContentContainer: {
        display: 'flex',
        flexDirection: 'column',
        flexGrow: 1,
        justifyContent: 'center',
        textAlign: 'center',
        margin: '2vh 2vw 2vh 2vw',
    },

    secondaryContentContainer: {
        display: 'flex',
        flexGrow: 1,
        alignItems: 'center',
        flexDirection: 'column',
        justifyContent: 'space-around',
    },

    actionContainer: {
        display: 'flex',
        justifyContent: 'flex-end',
        margin: '0 2vw 2vh 2vw',
        textAlign: 'center',
    },

    buttonNotFilled: {
        fontFamily: 'MetropolisMedium',
        fontSize: '1rem',
        borderRadius: '8px',
        '& span': {
            color: theme.palette.primary.dark,
        },
        textTransform: 'capitalize',
        flexGrow: 1,
        margin: '0 0.5vw 0 0.5vw',
    },

    buttonFilled: {
        fontFamily: 'MetropolisMedium',
        fontSize: '1rem',
        borderRadius: '8px',
        backgroundColor: theme.palette.primary.dark,
        '&:hover': {
            backgroundColor: theme.palette.primary.dark,
        },
        '& span': {
            color: '#FFFF',
        },
        textTransform: 'capitalize',
        flexGrow: 1,
        margin: '0 0.5vw 0 0.5vw',
    },
}));

export default useGlobalStyles;
