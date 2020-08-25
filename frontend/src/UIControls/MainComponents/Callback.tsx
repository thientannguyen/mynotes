import React from 'react';
import {
    Backdrop,
    CircularProgress,
    Typography,
    Grid,
} from '@material-ui/core';
import useStyles from '../../Themes/MainComponents/CallbackTheme';

const Callback = () => {
    const classes = useStyles();
    return (
        <div>
            <Backdrop className={classes.backdrop} open={true}>
                <Grid
                    container
                    direction="column"
                    justify="center"
                    alignItems="center"
                >
                    <Grid item>
                        <CircularProgress color="inherit" />
                    </Grid>
                    <Grid item>
                        <Typography
                            className={classes.loadingText}
                            variant="h6"
                            gutterBottom
                        >
                            Loading...
                        </Typography>
                    </Grid>
                </Grid>
            </Backdrop>
        </div>
    );
};

export default Callback;
