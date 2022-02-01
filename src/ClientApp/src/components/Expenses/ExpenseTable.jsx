import './ExpenseTable.css';
import { expenseConst } from '../../_constants';

import React, { useState, useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { Link } from 'react-router-dom';

import ExpenseRow from './ExpenseRow';

import Table from 'react-bootstrap/Table';
import Col from 'react-bootstrap/Col';
import InputGroup from 'react-bootstrap/InputGroup';
import FormControl from 'react-bootstrap/FormControl';
import Button from 'react-bootstrap/Button';
import axios from 'axios';


function ExpenseTable(){
    const dispatch = useDispatch();
    const expenseStore = useSelector(store => store.expenses.expenseReducer);
    const expenses = expenseStore.listItem;
    useEffect(() => {
        dispatch({ type: expenseConst.FETCH_ALL });
    }, [dispatch]);

    return (
        
        <Col className="align-items-center justify-content-center">
        <Table bordered hover className="expenseTable">
            <thead>
                <tr>
                    Purchase Date
                </tr>
                <tr>
                    Amount
                </tr>
                <tr>
                    Merchant
                </tr>
                <tr>
                    Sub Category
                </tr>
                <tr>
                    Main Category
                </tr>
            </thead>
            {expenseStore.isLoading === false ? expenseStore.listItem.Map(expense => {
                return (
                    <tbody key={expense.Id}>
                        <ExpenseRow
                        expense={expense}
                        />
                    </tbody>
                );
            }) : <div>Loading...</div>}
        </Table>
        </Col>
    );
}

export default ExpenseTable;