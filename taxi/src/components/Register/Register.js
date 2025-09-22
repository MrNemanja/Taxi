import "./Register.css";
import { useState, useEffect } from "react";
import { useNavigate, Link } from "react-router-dom";
import CallServerRegister from "../Services/RegisterService";
import { GoogleLogin } from "react-google-login";
import { gapi } from "gapi-script";


function Register() {

    const [username, setUsername] = useState('');
    const [usernameErr, setUsernameErr] = useState(true);
    const [email, setEmail] = useState('');
    const [emailErr, setEmailErr] = useState(true);
    const [password, setPassword] = useState('');
    const [passwordErr, setPasswordErr] = useState(true);
    const [confirmPassword, setConfirmPassword] = useState('');
    const [confirmPasswordErr, setConfirmPasswordErr] = useState(true);
    const [name, setName] = useState('');
    const [nameErr, setNameErr] = useState(true);
    const [surname, setSurname] = useState('');
    const [surnameErr, setSurenameErr] = useState(true);
    const [address, setAddress] = useState('');
    const [addressErr, setAddressErr] = useState(true);
    const [dateOfBirth, setDateOfBirth] = useState('');
    const [dateOfBirthErr, setDateOfBirthErr] = useState(true);
    const [role, setRole] = useState("RegularUser");
    const [imageUrl, setImageUrl] = useState('');
    const [imageUrlErr, setImageUrlErr] = useState(true);

    const navigate = useNavigate();
    const clientID = process.env.REACT_APP_CLIENT_ID;

    const clickRegister = async (e) =>
    {
        e.preventDefault();
        
        const result = await CallServerRegister(username, usernameErr, email, emailErr, password, passwordErr, confirmPassword, confirmPasswordErr,
            name, nameErr, surname, surnameErr, address, addressErr, dateOfBirth, dateOfBirthErr, role, imageUrl, imageUrlErr);
        

        if(result === true)
        {   
            alert("Your registration is successfull");
            navigate("/");
        }
    }
    
    const inputChanged = (set, setErr) => (e) =>
    {
        const value = e.target.value;
        set(value);
        setErr(value.trim() === "");
    }

    const dateChanged = (e) =>
    {   
        const value = e.target.value;
        
        if(value.trim() === "") setDateOfBirthErr(true);
        else
        {
            const today = new Date();
            const tYear = today.getFullYear();
            const tMonth = today.getMonth() + 1;
            const tDay = today.getDate();

            const values = value.split('-');
            const year = parseInt(values[0]);
            const month = parseInt(values[1]);
            const day = parseInt(values[2]);
        

            if(year > tYear) setDateOfBirthErr(true);
            else if(year === tYear)
            {
                if(month > tMonth) setDateOfBirthErr(true);
                else if(month === tMonth)
                {
                    if(day > tDay) setDateOfBirthErr(true)
                    else setDateOfBirthErr(false);
                }
                else setDateOfBirthErr(false);
            } 
            else setDateOfBirthErr(false);  

            setDateOfBirth(value);
        }
        
    }

    const emailChanged = (e) =>
    {
        const value = e.target.value;
        setEmail(value);
        const isValidEmail = /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value);
        setEmailErr(!isValidEmail);

    }

    const confirmPasswordChanged = (e) => {
        const value = e.target.value;
        setConfirmPassword(value);
        setConfirmPasswordErr(value !== password);
    };

    const roleChanged = (e) => 
    {
        const value = e.target.value;
        setRole(value);
    }

    const imageUrlChanged = (e) => {
        const file = e.target.files[0];
        setImageUrl(file || null);
        setImageUrlErr(!file);
    };

    useEffect(() => {
        if (clientID) {
            function start() {
                gapi.client.init({
                    clientId: clientID,
                    scope: ""
                });
            }
            gapi.load('client:auth2', start);
        } else {
            console.error("Client ID is not defined in .env file");
        }
    });

    const onSuccess = (response) => 
    {
        const profile = response.profileObj;
        const Username = profile.email;
        const Email = profile.email;
        const Name = profile.givenName;
        const Surname = profile.familyName;
        const ImageURL = profile.imageUrl;

        navigate('/googleRegister', {
            state:
            {
                username : Username,
                email: Email,
                name: Name,
                surname: Surname,
                imageUrl: ImageURL,
            }
        })
        
    }

    const onFailure = (response) =>
    {
        console.log("Failed to register: ", response);
    }


    return(
        <div className="App">
            <div className='Register'>
                <h1>Register</h1>
                <form onSubmit={clickRegister} method='POST'>
                    <label className='label'>Username: &nbsp;</label>
                    <input type='text' placeholder="Type username..." onChange={inputChanged(setUsername, setUsernameErr)} value={username || ''}></input>
                    <br />
                    <br />
                    <label className='label'>Email: &nbsp;</label>
                    <input type='text' placeholder="Type email..." onChange={emailChanged} value={email || ''}></input>
                    <br />
                    <br />
                    <label className='label'>Password: &nbsp;</label>
                    <input type='password' placeholder="Type password..." onChange={inputChanged(setPassword, setPasswordErr)} value={password || ''}></input>
                    <br />
                    <br />
                    <label className='label'>Confirm Password: &nbsp;</label>
                    <input type='password' placeholder="Type confirm password..." onChange={confirmPasswordChanged} value={confirmPassword || ''}></input>
                    <br />
                    <br />
                    <label className='label'>Name: &nbsp;</label>
                    <input type='text' placeholder="Type name..." onChange={inputChanged(setName, setNameErr)} value={name || ''}></input>
                    <br />
                    <br />
                    <label className='label'>Surname: &nbsp;</label>
                    <input type='text' placeholder="Type surname..." onChange={inputChanged(setSurname, setSurenameErr)} value={surname || ''}></input>
                    <br />
                    <br />
                    <label className='label'>Address: &nbsp;</label>
                    <input type='text' placeholder="Type address..." onChange={inputChanged(setAddress, setAddressErr)} value={address || ''}></input>
                    <br />
                    <br />
                    <label className='label'>Date of birth: &nbsp;</label>
                    <input type="date" onChange={dateChanged} value={dateOfBirth || ''}></input>
                    <br />
                    <br />
                    <label className='label'>Role: &nbsp;</label>
                    <select onChange={roleChanged} value={role}>
                        <option>Driver</option>
                        <option>RegularUser</option>
                    </select>
                    <br />
                    <br />
                    <label className='label'>Image: &nbsp;</label>
                    <input type="file" onChange={imageUrlChanged}></input>
                    <br />
                    <br />
                    <button className='button' type='submit' value='Register'>Register</button>
                    <br />
                    <br />
                    <div className="google">
                    <GoogleLogin 
                    clientId = {clientID}
                    buttonText="Register with google"
                    onSuccess={onSuccess}
                    onFailure={onFailure}
                    cookiePolicy={'single_host_origin'}
                    theme="dark"
                    />  
                    </div>   
                    <br />
                    <br />      
                     <p className="p">Already have an account?&nbsp;</p>
                    <Link className="link" to="/">Try to LogIn.</Link>
                    

                </form>
            </div>
        </div>

    );

}

export default Register;