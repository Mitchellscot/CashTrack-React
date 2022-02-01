import { put, takeLatest } from 'redux-saga/effects';
import axios from 'axios';
import { expenseConst } from '../../_constants';

function* fetchAllExpenses() {
    try {
        const expenseResponse = yield axios.get('/api/expense?dateoptions=1');
        yield put({type: expenseConst.SET_EXPENSES, payload: {
            totalAmount: expenseResponse.data.totalAmount,
            pageNumber: expenseResponse.data.pageNumber,
            pageSize: expenseResponse.data.pageSize,
            totalCount: expenseResponse.data.totalCount,
            totalPages: expenseResponse.data.totalPages,
            listItems: expenseResponse.data.listItems
        }})
    } catch (error) {
        console.log(`HEY MITCH - COULDN'T GET THE EXPENSES ${error}`);
    }
}

function* expenseSaga() {
    yield takeLatest(expenseConst.FETCH_ALL, fetchAllExpenses);
  }

export default expenseSaga;