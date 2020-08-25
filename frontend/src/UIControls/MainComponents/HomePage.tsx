import React, { useEffect, useState } from 'react';
import { Button, Typography } from '@material-ui/core';
import AuthService from '../../Services/AuthService';
import useGlobalStyles from '../../Themes/GlobalStyle';
import CookiesService from '../../Services/CookiesService';
import useStyles from '../../Themes/MainComponents/HomePageTheme';
import UserManagementService from '../../Services/UserManagementService';
import { User } from '../../Models/User/User';

const HomePage = () => {
    const globalStyles = useGlobalStyles();
    const classes = useStyles();
    const [user, setUser] = useState<User>();

    const handleClick = () => {
        AuthService.getInstance().logout();
    };

    useEffect(() => {
        const updateUserId = async () => {
            const token = await AuthService.adB2cInstance.acquireAccessToken();
            const tempUser = await UserManagementService.getUser(
                CookiesService.serviceInstance.getUserId(),
                token
            );
            setUser(tempUser);
        };
        updateUserId();
    }, [user]);

    return (
        <div className={globalStyles.baseFrameContainer}>
            <div className={globalStyles.primaryContentContainer}>
                <Typography
                    className={classes.headerText}
                    id="txt-title"
                    variant="h5"
                    gutterBottom
                >
                    This is {user?.name}'s Notes Homepage.
                </Typography>
            </div>

            <div className={globalStyles.actionContainer}>
                <Button
                    id="btn-logout"
                    variant="contained"
                    className={globalStyles.buttonFilled}
                    onClick={handleClick}
                >
                    Log out
                </Button>
                <Button
                    id="btn-addNote"
                    variant="contained"
                    className={globalStyles.buttonFilled}
                >
                    Add Note
                </Button>
            </div>
        </div>
    );
};

export default HomePage;
