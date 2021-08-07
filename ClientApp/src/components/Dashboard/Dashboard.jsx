import React/*, { useState, useEffect }*/ from 'react';
//import { useDispatch } from 'react-redux';
import {useSelector} from 'react-redux';

function Dashboard(){
    const user = useSelector((store) => store.login);
    //const dispatch = useDispatch();

/*     useEffect({
    }, []) */

    return(
        <div className="text-center">
        {user.loggedIn ? <h1>HI {JSON.stringify(user.user.firstName)}</h1> : <h1>Something went wrong...</h1>}
        {user.loggedIn ? <h1>You are logged in</h1> : <h1>You are not logged in</h1> }
        {user.loggedIn ? <h1>This is your role: {JSON.stringify(user.user.role)}</h1> : <h1>You have not role</h1> }
            </div>
    );
}

export default Dashboard;