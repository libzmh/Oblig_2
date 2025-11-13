import { Navbar, Nav, Container } from 'react-bootstrap';
import { Link } from 'react-router-dom';

const NavMenu = () => {
  return (
    <Navbar bg="white" expand="sm" className="border-bottom box-shadow mb-3">
      <Container fluid>
        <Navbar.Brand as={Link} to="/">MyQuiz</Navbar.Brand>
        <Navbar.Toggle aria-controls="basic-navbar-nav" />
        <Navbar.Collapse id="basic-navbar-nav">
          <Nav className="flex-grow-1">
            <Nav.Link as={Link} to="/">Home</Nav.Link>
          </Nav>
        </Navbar.Collapse>
      </Container>
    </Navbar>
  );
};

export default NavMenu;