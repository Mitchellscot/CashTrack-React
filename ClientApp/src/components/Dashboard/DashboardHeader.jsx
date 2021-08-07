import './Dashboard.css';
import Navbar from 'react-bootstrap/Navbar';
import { BoxArrowRight } from 'react-bootstrap-icons';
import { useDispatch } from 'react-redux';
import { loginConst } from "../../_constants";

function DashboardHeader(){
    const dispatch = useDispatch();

    return(
        <Navbar sticky="top" bg="dark" expand="lg" className="d-flex justify-content-between flex-md-nowrap p-0 shadow">
            <a className="d-flex justify-content-center navbar-brand col-md-2 col-lg-2 md-0 px-2 py-2">
                <img src="../../public/images/cash-track-text.png" height="45px" width="200px" alt="logo" /></a>
            <ul className="navbar-nav px-3">
                <li className="nav-item text-nowrap">
                    <BoxArrowRight 
                    id="logoutButton"
                    className="mr-4"
                    fontSize="2rem"
                    color="white"
                    onClick={() => dispatch({ type: loginConst.LOGOUT })}
                    />
                </li>
            </ul>
        </Navbar>
    );
}

export default DashboardHeader;