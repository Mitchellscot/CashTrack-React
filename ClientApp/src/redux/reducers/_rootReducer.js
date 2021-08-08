import { combineReducers } from 'redux';
import login from './loginReducer';
import errors from './errorsReducer';

const rootReducer = combineReducers({
    login,
    errors
});

export default rootReducer;