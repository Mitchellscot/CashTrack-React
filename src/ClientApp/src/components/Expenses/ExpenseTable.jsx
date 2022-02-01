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

function ExpenseTable() {
    const dispatch = useDispatch();
    const expenseStore = useSelector(store => store.expenses.expenseReducer);

    useEffect(() => {
        dispatch({ type: expenseConst.FETCH_ALL });
    }, [dispatch]);

    return (
        <Col className="align-items-center justify-content-center">
            <Table bordered hover className="expenseTable">
                <thead>
                    <tr>
                        <th>
                            Purchase Date
                        </th>
                        <th>
                            Amount
                        </th>
                        <th>
                            Merchant
                        </th>
                        <th>
                            Sub Category
                        </th>
                        <th>
                            Main Category
                        </th>
                    </tr>
                </thead>
                    {expenseStore.isLoading === true ? <div>Loading...</div> : expenseStore.listItems.map(expense => {
                        return (
                            <tbody key={expense.Id}>
                                <ExpenseRow
                                    expense={expense}
                                />
                            </tbody>
                        );
                    })}
            </Table>
        </Col>
    );
}

export default ExpenseTable;