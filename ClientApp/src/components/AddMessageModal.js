import React, {Component} from 'react';
import {Modal,Button, Row, Col, Form, Image } from 'react-bootstrap';
import authService from './api-authorization/AuthorizeService';

export class AddMessageModal extends Component {
    constructor(props) {
        super(props);
        this.handleSubmit=this.handleSubmit.bind(this);
        this.handleFileSelected=this.handleFileSelected.bind(this);
    }

    // Paden voor het opslaan van de afbeelding
    imagefilename = "empty.png";
    imagesrc = process.env.REACT_APP_IMAGESPATH + this.imagefilename;

    // Versturen van het bericht
    handleSubmit(event) {
        event.preventDefault();

        const {userName} = this.state;
        const image = this.imagesrc.endsWith("empty.png") ? '' : this.imagesrc;

        fetch(process.env.REACT_APP_API + 'message', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-type': 'application/json'
            },
            body:JSON.stringify({
                Content: event.target.messageContent.value,
                UserId: userName,
                Image: image,
                Likes: []
            })
        })
        .then(res => res.json())
        .then((result) => {
            this.imagesrc = 'Empty.png';
            this.props.onHide(event);
        },
        (error) => {
            alert('Fout');
        })
    }

    componentDidMount() {
        this._subscription = authService.subscribe(() => this.populateState());
        this.populateState();
    }
  
    componentWillUnmount() {
        authService.unsubscribe(this._subscription);
    }
  
    async populateState() {
        const [isAuthenticated, user] = await Promise.all([authService.isAuthenticated(), authService.getUser()])

        if (isAuthenticated) {
            this.setState({
                userName: user.name,
            });
        }
    }

    // Verwerken van afbeelding voor het bericht
    handleFileSelected(event) {
        event.preventDefault();

        const formData = new FormData();
        console.log(event.target.files[0]);
        formData.append(
            "myFile",
             event.target.files[0],
             event.target.files[0].name
        );

        fetch(process.env.REACT_APP_API + 'message/SaveFile', {
            method: 'POST',
            body:formData
        })
        .then(res => res.json())
        .then((result) => {
            this.imagesrc = process.env.REACT_APP_IMAGESPATH + result;
        },
        (error) => {
            alert('Fout bij het uploaden van de afbeelding');
        })
    }

    render() {
        return (
            <div className="container">
                <Modal
                    {...this.props}
                    size="lg"
                    aria-labelledby="contained-modal-title-vcenter"
                    centered>
                    <Modal.Header closeButton>
                        <Modal.Title id="contained-modal-title-vcenter">
                            Nieuw bericht
                        </Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <Row>
                            <Col>
                                <Form onSubmit={this.handleSubmit} >
                                    <Form.Group controlId="Title">
                                        <Form.Label>Vul hier de tekst in:</Form.Label>
                                        <Form.Control as="textarea" rows={6} name="messageContent" required/>
                                        <Image className="mt-2 mb-2" width="100px" height="100px" src={this.imagesrc}/>
                                        <Form.Control type="File" name="messageImage" onChange={this.handleFileSelected}/>
                                    </Form.Group>
                                    <Form.Group>
                                        <Button className="mt-3" variant="primary" type="submit">
                                            Opslaan
                                        </Button>
                                    </Form.Group>
                                </Form>
                            </Col>
                        </Row>
                    </Modal.Body>     
                    <Modal.Footer>
                        <Button variant="danger" onClick={this.props.onHide}>Sluiten</Button>
                    </Modal.Footer>
                </Modal>
            </div>
        )
    }
}