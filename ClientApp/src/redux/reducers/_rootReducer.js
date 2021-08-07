import { combineReducers } from 'redux';
import login from './loginReducer';
import loginAlert from './loginAlertReducer';

const rootReducer = combineReducers({
    login,
    loginAlert
});

export default rootReducer;