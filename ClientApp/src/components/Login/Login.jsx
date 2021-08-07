import React, { useState, useEffect } from 'react';
import LoginForm from '../LoginForm/LoginForm';
import LoginForgotPassword from '../LoginForgotPassword/LoginForgotPassword';
import { useSelector, useDispatch } from 'react-redux';
import './Login.css';


export default function Login() {
    const alert = useSelector(store => store.loginAlert);
    const dispatch = useDispatch();
    const [forgotPassword, setForgotPassword] = useState(false);
    useEffect(() => {
        dispatch({ type: 'ALERT_CLEAR' });
        dispatch({ type: 'LOGOUT' });
    }, []);

    return (
        <div className="container">

            <div className="col-md-8 offset-md-2 mt-5">
                <div className="col-lg-8 offset-lg-2 border rounded">
                    <div className="d-flex justify-content-center navbar-brand mt-3">
                        <img src="\images\cash-track.png" height="250px" width="250px" alt="Cash Track Logo" />
                    </div>
                    {alert.message &&
                            <div className={`alert ${alert.type} text-center py-0 mx-3`}>{alert.message}</div>
                        }
                        {forgotPassword ? 
                        <LoginForgotPassword /> : 
                        <LoginForm setForgotPassword={setForgotPassword}/>
                    }
                    

                </div>

            </div>
            <div className="col-lg-6 offset-lg-3 rounded mt-0">

                    </div>
        </div>
    );
}