import './Dashboard.css';
import Container from 'react-bootstrap/Container';
import Col from 'react-bootstrap/Col';
import DashboardHeader from './DashboardHeader';
import DashboardSideBar from './DashboardSideBar';
import Row from 'react-bootstrap/Row';
import { Switch, Route } from "react-router-dom";

function Dashboard() {
    return (
        <>
            <DashboardHeader />
            <Container fluid>
                <Row>
                    <DashboardSideBar />
                    <Col md={10} lg={10} className="ml-sm-auto px-md-5 py-3">
                        <main className="d-flex flex-wrap flex-md-nowrap pt-4 border border-bottom-0 rounded-top">
{/*                         <Switch>
                        <Route path="/Admin/Orders">
                            <OrdersTable />
                            </Route>
                        <Route path="/Admin/Pictures">
                            <PicturesTable />
                            </Route>
                        <Route path="/Admin/Camera">
                            <Camera />
                        </Route>
                        <Route path="/Admin/Account">
                            <Account />
                        </Route>
                        </Switch> */}
                        </main>
                    </Col>
                </Row>
            </Container>
        </>
    );
}

export default Dashboard;