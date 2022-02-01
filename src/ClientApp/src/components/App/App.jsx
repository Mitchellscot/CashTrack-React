// dark mode
import "bootswatch/dist/darkly/bootstrap.min.css";
// light mode
//import "bootswatch/dist/flatly/bootstrap.min.css";
import React from 'react';
//import { Layout } from '../Layout/Layout';
import Login from '../Login/Login';
import { createBrowserHistory } from 'history';
import { useSelector } from 'react-redux';
import {
    BrowserRouter,
    Switch,
    Redirect,
    withRouter
} from 'react-router-dom'; //can also import <Route /> if needed
import ProtectedRoute from '../../_helpers/ProtectedRoute/ProtectedRoute';
import Dashboard from '../Dashboard/Dashboard';
import ExpenseTable from '../Expenses/ExpenseTable';

function App() {
    const user = useSelector(store => store.login.loggedIn);
    const history = createBrowserHistory();
    return (
        <div id={!user ? "login-page" : ""}>
            <BrowserRouter history={history}>
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
                    <ProtectedRoute
                        exact
                        path="/expenses">
                        <ExpenseTable />
                    </ProtectedRoute>
                </Switch>
            </BrowserRouter>
        </div>
    );
}

export default withRouter(App);