import { put, takeLatest } from 'redux-saga/effects';
import axios from 'axios';

function* loginUser(action){

    try {
        yield put({type: 'ALERT_CLEAR' });
        const config = {
            headers: { 'Content-Type': 'application/json' },
            withCredentials: true,
          };
          let user = {};
        yield axios.post('/user/authenticate', action.payload, config).then(response => {
                  localStorage.setItem('user', JSON.stringify(response.data));
                  user = response.data;
              });
        yield put({type: 'LOGIN_SUCCESS', user: user});
        yield put({type: 'ALERT_SUCCESS', message: "Success!"})
    } catch (error) {
        console.log('HEY MITCH - ERROR LOGGING IN', error);
        if (error.response.status === 401){
            yield put({ type: 'LOGIN_FAILURE' });
            yield put({ type: 'ALERT_ERROR', message: error.response.data.message})
        }
        else if (error.response.status === 400){
            yield put({ type: 'ALERT_ERROR', message: error.response})
            yield put({ type: 'LOGIN_FAILURE' });
        }
    }
}

function* logoutUser(){
    try {
        localStorage.removeItem('user');
        yield put({type: 'LOGOUT_USER'});
    } catch (error) {
        console.log('HEY MITCH - COULD NOT LOG OUT USER', error);
    }
}

function* loginSaga(){
    yield takeLatest('LOGIN', loginUser);
    yield takeLatest('LOGOUT', logoutUser);
}

export default loginSaga;