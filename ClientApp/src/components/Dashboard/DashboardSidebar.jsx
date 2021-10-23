import './Dashboard.css';
import Nav from 'react-bootstrap/Nav';
import { InboxFill, CalculatorFill, GearFill, BarChartFill, CashStack } from 'react-bootstrap-icons';
import { Link, useRouteMatch } from "react-router-dom";

function DashboardSidebar() {
    let match = useRouteMatch();
    return (
        <Nav className="sidebar col-md-2 col-lg-2 d-md-block bg-light collapse position-sticky pt-3" >
             <Nav defaultActiveKey="Orders" as="ul" className="nav flex-column align-items-center">
                <Nav.Item as="li">
                    <Link className="nav-link" role="button" to={`/Budget`}>
                        Budget&nbsp;&nbsp;&nbsp;&nbsp;
                        <CalculatorFill />
                    </Link>
                </Nav.Item>
                <Nav.Item as="li">
                    <Link className="nav-link" role="button" to={`${match.url}/Expenses`}>
                        Expenses&nbsp;&nbsp;
                        <InboxFill />
                    </Link>
                </Nav.Item>
                <Nav.Item as="li">
                    <Link className="nav-link" role="button" to={`${match.url}/Income`}>
                        Income&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <CashStack />
                    </Link>
                </Nav.Item>
                <Nav.Item as="li">
                    <Link className="nav-link" role="button" to={`${match.url}/Reports`}>
                        Reports&nbsp;&nbsp;&nbsp;
                        <BarChartFill />
                    </Link>
                </Nav.Item>
                <Nav.Item as="li">
                    <Link className="nav-link" role="button" to={`${match.url}/Settings`}>
                        Settings&nbsp;
                        <GearFill />
                    </Link>
                </Nav.Item>

            </Nav>
        </Nav>
    );
}

export default DashboardSidebar;