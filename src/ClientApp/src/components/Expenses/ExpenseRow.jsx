import './ExpenseRow.css';
import React, { useState } from 'react';
import { useDispatch } from 'react-redux';
import axios from 'axios';
import Modal from 'react-bootstrap/Modal';

function ExpenseRow({ expense }) {
    const [modal, setModal] = useState(false);
    const handleShowModal = () => {
        setModal(!modal);
    }

    return (
        <tr>
            <td className="align-middle text-center">
                {expense.date}
            </td>
            <td className="align-middle text-center">
            {expense.amount}
            </td>
            <td className="align-middle text-center">
            {expense.merchant}
            </td>
            <td className="align-middle text-center">
            {expense.subCategory}
            </td>
            <td className="align-middle text-center">
            {expense.mainCategory}
            </td>
        </tr>

    );
}

export default ExpenseRow;