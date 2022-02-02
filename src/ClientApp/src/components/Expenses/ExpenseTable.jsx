import './ExpenseTable.css';
import { expenseConst } from '../../_constants';

import React, { useState, useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { Link } from 'react-router-dom';

import ExpenseRow from './ExpenseRow';
import ExpensePagination from './ExpensePagination';

import Table from 'react-bootstrap/Table';
import Col from 'react-bootstrap/Col';
import Row from 'react-bootstrap/Row';

import Container from 'react-bootstrap/Container';
import InputGroup from 'react-bootstrap/InputGroup';
import FormControl from 'react-bootstrap/FormControl';
import Button from 'react-bootstrap/Button';
import axios from 'axios';

function ExpenseTable() {
    const dispatch = useDispatch();
    const expenseStore = useSelector(store => store.expenses.expenseReducer);

    useEffect(() => {
        expenseStore.listItems.length === 0 && dispatch({ type: expenseConst.FETCH_ALL });
    }, []);

    return (
        <Container fluid>
            <Col className="align-items-center justify-content-center">
                <Table bordered hover className="expenseTable" size="sm">
                    <thead>
                        <tr class="table-primary">
                            <th scope="col">
                                Purchase Date
                            </th>
                            <th scope="col">
                                Amount
                            </th>
                            <th scope="col">
                                Merchant
                            </th>
                            <th scope="col">
                                Sub Category
                            </th>
                            <th scope="col">
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
                <Row>
                    <Col md={{ span: 2, offset: 0 }}><ExpensePagination expenseStore={expenseStore} /></Col>
                </Row>
            </Col>
        </Container>
    );
}

export default ExpenseTable;