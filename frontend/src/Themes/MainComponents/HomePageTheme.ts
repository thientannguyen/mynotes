import { makeStyles } from '@material-ui/core/styles';

const useStyles = makeStyles((theme) => ({
    headerText: {
        fontFamily: 'MetropolisSemiBold',
        fontSize: '2rem',
        color: theme.palette.primary.dark,
    },
}));
export default useStyles;
