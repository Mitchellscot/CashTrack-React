import { useHistory } from 'react-router-dom';
import React, { useState } from 'react';

export default function LoginForgotPassword() {
    const history = useHistory();
    const [email, setEmail] = useState('');
    const [submitted, setSubmitted] = useState(false);

    const handleSubmit = () => {
        setSubmitted(true);
        alert("Check your email");
    }

    return (
        <>
            <form name="form" onSubmit={handleSubmit}>
                <div className="form-row d-flex">
                    <div className="col mx-2">
                        <input placeholder="Email" className="form-control" id="emailInput" required type="text" name="email" value={email} onChange={(e) => setEmail(e.target.value)} /></div>
                    {submitted && !email && <div className="invalid-feedback">Email is required</div>}
                </div>
                <div className="form-group d-flex justify-content-between m-2">
                    <button className="btn btn-primary btn" type="submit" name="send">
                        Send me an Email
                    </button>
                    <button
                        onClick={() => history.push('/login')}
                        id="backToLogin" type="button" className="btn btn-link btn">Return to Login</button>
                </div>
            </form>
        </>
    );
}