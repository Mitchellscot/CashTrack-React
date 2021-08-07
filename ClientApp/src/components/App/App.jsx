// dark mode
//import "bootswatch/dist/darkly/bootstrap.min.css";
// light mode
import "bootswatch/dist/flatly/bootstrap.min.css";
import React from 'react';
//import { Layout } from '../Layout/Layout';
import Login from '../Login/Login';
import { createBrowserHistory } from 'history';
import {
    Router,
    Redirect,
    Switch,
} from 'react-router-dom'; //can also import <Route /> if needed
import ProtectedRoute from '../../_helpers/ProtectedRoute/ProtectedRoute';
import Dashboard from '../Dashboard/Dashboard';


function App() {
    const history = createBrowserHistory();
    return (
        <Router history={history}>
            <Switch>
            <Redirect exact from="/" to="/login" />
                <ProtectedRoute
                    path="/login"
                    authRedirect="/dashboard"
                    >
                        <Login />
                    </ProtectedRoute>
                <ProtectedRoute
                exact
                path="/dashboard">
                    <Dashboard />
                </ProtectedRoute>
            </Switch>
        </Router>
    );
}

export default App;