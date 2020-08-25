import React, { useState, useEffect } from 'react';
import Button from '@material-ui/core/Button';
import AuthService from '../../Services/AuthService';
import { useHistory } from 'react-router';
import Typography from '@material-ui/core/Typography';
import useGlobalStyles from '../../Themes/GlobalStyle';
import useStyles from '../../Themes/MainComponents/LoginPageTheme';

export default function LoginPage() {
    const globalStyles = useGlobalStyles();
    const classes = useStyles();
    const [isContinue, setIsContinue] = useState(false);
    const history = useHistory();

    useEffect(() => {
        const isAuth = AuthService.getInstance().getIsAuthenticated();
        setIsContinue(isAuth);
    }, [isContinue]);

    const handleClick = async () => {
        if (!isContinue) {
            console.log('Starting route to AD B2C');
            await AuthService.getInstance().login('/');
        } else {
            history.push('/');
        }
    };

    return (
        <div className={globalStyles.baseFrameContainer}>
            <div className={globalStyles.primaryContentContainer}>
                <Typography
                    className={classes.loginText}
                    variant="h3"
                    gutterBottom
                >
                    MY NOTES LOGIN
                </Typography>
                <br />
                <Typography
                    className={classes.loginText}
                    variant="h5"
                    gutterBottom
                >
                    Please SIGN UP or LOG IN first!
                </Typography>
            </div>
            <div className={globalStyles.actionContainer}>
                <Button
                    id="btn-start"
                    variant="contained"
                    className={globalStyles.buttonFilled}
                    onClick={handleClick}
                >
                    Start
                </Button>
            </div>
        </div>
    );
}
