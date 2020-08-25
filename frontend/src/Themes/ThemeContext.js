import React from 'react';
import {
    createMuiTheme,
    makeStyles,
    responsiveFontSizes,
} from '@material-ui/core/styles';

const theme = responsiveFontSizes(
    createMuiTheme({
        palette: {
            primary: {
                // main blue
                light: '#c2ced4',
                main: '#658494',
                dark: '#44677a',
            },
            secondary: {
                //orange
                main: '#ec6628',
            },
            error: {
                main: '#ff3b30',
            },
            success: {
                main: '#adaf17',
            },
            info: {
                main: '#008bd2',
            },
            warning: {
                // orange accent
                main: '#ec6628',
            },
        },
        typography: {
            fontFamily: 'Noto Sans',
            h5: {
                fontWeight: 500,
                fontSize: 26,
                letterSpacing: 0.5,
            },
        },
        shape: {
            borderRadius: 8,
        },
        props: {
            MuiTab: {
                disableRipple: true,
            },
        },
    })
);

const useStyles = makeStyles({
    root: {
        display: 'flex',
        height: '100%',
        paddingBottom: '60px',
    },
    drawer: {
        [theme.breakpoints.up('sm')]: {
            width: 256,
            flexShrink: 0,
        },
    },
    app: {
        flex: 1,
        display: 'flex',
        flexDirection: 'column',
    },
    main: {
        flex: 1,
        padding: theme.spacing(6, 2, 0),
        height: '90vh',
    },
    footer: {
        alignSelf: 'center',
        color: theme.palette.secondary.main,
        padding: theme.spacing(2),
        bottom: '0',
        position: 'relative',
    },
    layout: {
        width: 'auto',
        marginLeft: theme.spacing(2),
        marginRight: theme.spacing(2),
        [theme.breakpoints.up(600 + theme.spacing(2) * 2)]: {
            width: 600,
            marginLeft: 'auto',
            marginRight: 'auto',
        },
    },
    paper: {
        marginTop: theme.spacing(3),
        marginBottom: theme.spacing(3),
        padding: theme.spacing(2),
        [theme.breakpoints.up(600 + theme.spacing(3) * 2)]: {
            marginTop: theme.spacing(6),
            marginBottom: theme.spacing(6),
            padding: theme.spacing(3),
        },
    },
    gridItem: {
        padding: theme.spacing(2),
        color: theme.palette.text.secondary,
        alignItems: 'center',
    },
    button: {
        marginTop: theme.spacing(3),
        marginLeft: theme.spacing(1),
    },
    mainContainer: {
        textAlign: 'center',
        display: 'flex',
        alignItems: 'center',
    },
});

const ThemeContext = React.createContext({
    theme,
    useStyles,
});

export function ThemeProvider(props) {
    return (
        <ThemeContext.Provider
            value={{
                theme,
                useStyles,
            }}
        >
            {props.children}
        </ThemeContext.Provider>
    );
}

export default ThemeContext;
