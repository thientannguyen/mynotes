import React, { useContext } from 'react';
import './App.css';
import { BrowserRouter, Switch, Route } from 'react-router-dom';
import LoginPage from './UIControls/MainComponents/LoginPage';
import ThemeContext from './Themes/ThemeContext';
import { ThemeProvider } from '@material-ui/core/styles';
import AuthService from './Services/AuthService';
import HomePage from './UIControls/MainComponents/HomePage';
import Callback from './UIControls/MainComponents/Callback';

const getIsAuthenticated = () => {
    const result = AuthService.getInstance().getIsAuthenticated();
    console.log('Authentication is: ' + result);
    return result;
};

const App = () => {
    const { theme } = useContext(ThemeContext);
    return (
        <ThemeProvider theme={theme}>
            <div className="App-body">
                <BrowserRouter>
                    <Switch>
                        <Route path="/callback">
                            <Callback />
                        </Route>
                        <Route
                            path="/"
                            render={(props) =>
                                getIsAuthenticated() ? (
                                    <HomePage />
                                ) : (
                                    <LoginPage />
                                )
                            }
                        />
                    </Switch>
                </BrowserRouter>
            </div>
        </ThemeProvider>
    );
};

export default App;
