import React,{Component} from 'react';
import {Button,ButtonToolbar,Container, Col, Row, Image} from 'react-bootstrap';
import {AddMessageModal} from './AddMessageModal';
import authService from './api-authorization/AuthorizeService';

export class Messages extends Component {

    constructor(props) {
        super(props);
        this.state = {
          messages: [],
          addModalShow: false,
          userName: ''
        };
    }

    // Berichten ophalen
    async refreshList() {
      fetch(process.env.REACT_APP_API + 'Message')
      .then(response => response.json())
      .then(data => {
        this.setState({messages: data})
      })
      .catch((error) => {
        console.log(error);
      });

    }

    // Login checken
    componentDidMount() {
      this._subscription = authService.subscribe(() => this.populateState());
      this.populateState();
      this.refreshList();
    }

    componentDidUpdate() {
      this.refreshList();
    }

    componentWillUnmount() {
      authService.unsubscribe(this._subscription);
    }

    // Ingelogde gebruiker opslaan
    async populateState() {
      const [isAuthenticated, user] = await Promise.all([authService.isAuthenticated(), authService.getUser()])

      if (isAuthenticated) {
          this.setState({
              userName: user.name,
          });
      }
    }

    likeMessage(messageid) {

      const {userName} = this.state;

      fetch(process.env.REACT_APP_API + 'like', {
          method: 'POST',
          headers: {
              'Accept': 'application/json',
              'Content-type': 'application/json'
          },
          body:JSON.stringify({
              MessageId: messageid,
              UserId: userName
          })
      })
      .then(res => res.json())
      .then((result) => {
      },
      (error) => {
          alert('Fout');
      })
    }

    deleteMessage(messageid) {
      if (window.confirm('Weet je dit heel erg ontzettend zeker?')) {
        fetch(process.env.REACT_APP_API + 'message/' + messageid, {
          method: 'DELETE',
          header: {'Accept': 'application/json', 'Content-Type': 'application/json'}
        })
      }
    }

    render() {
        const {messages, userName} = this.state;
        let addModalClose=()=>this.setState({addModalShow:false});

        return(
          <div className="container">
            {messages.length < 1 ? <p>Er zijn nog geen berichten.</p> : ''}
            {messages.map(mes=>
              <Container fluid className="border border-primary mb-3">
                <Row className="text-light bg-secondary">
                  <Col className="fw-bold col-10">
                    {mes.userId} <i>({mes.creationDateString})</i>
                  </Col>
                  <Col className="text-end"><Button variant="danger" onClick={()=> this.deleteMessage(mes.id)}> X </Button></Col>
                </Row>
                <Row>
                  <Col>{mes.content}</Col>
                </Row>
                {mes.image !== ''
                  ? <Row className="mh-20"><Col><Image width="15%" height="15%" src={mes.image} className="img-fluid"></Image></Col> </Row>
                  : ''
                }
                <Row>
                  <Col className="text-end"> {mes.aantalLikes} <a class="bi-star" href="#" onClick={()=> this.likeMessage(mes.id)}/></Col>
                </Row>
                <ButtonToolbar>
                  
                </ButtonToolbar>
              </Container>
            )}
          
            {userName !== '' ? 
              <ButtonToolbar className="mt-3">
                <Button variant='primary' onClick={() => this.setState({addModalShow:true})}>
                  Nieuw bericht
                </Button>
                <AddMessageModal show={this.state.addModalShow} onHide={addModalClose}/>
              </ButtonToolbar>
              : ''
            }
          </div>
        )
      }
}