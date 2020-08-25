import { makeStyles } from '@material-ui/core/styles';

const useStyles = makeStyles((theme) => ({
    loadingText: {
        fontFamily: 'MetropolisSemiBold',
        color: theme.palette.primary.light,
    },
    backdrop: {
        zIndex: theme.zIndex.tooltip + 1,
        color: '#fff',
    },
}));
export default useStyles;
