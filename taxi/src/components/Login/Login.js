import './Login.css';
import { Link, useNavigate } from 'react-router-dom'
import { useState, useEffect } from 'react';
import CallServerLogin from '../Services/LoginService';
import { jwtDecode } from 'jwt-decode';

function Login() { 

    const [username, setUsername] = useState('');
    const [usernameErr, setUsernameErr] = useState(true);
    const [password, setPassword] = useState('');
    const [passwordErr, setPasswordErr] = useState(true);

    const navigate = useNavigate();
    const token = localStorage.getItem('token');

    const clickLogin = async (e) => 
    {
        e.preventDefault();

        if(token !== null) 
        {
            alert("Someone is already logged into the system..");
        }
        else
        {
            const result = await CallServerLogin(username, usernameErr, password, passwordErr);

            if(result === "Invalid username or password")
            {
                alert("Invalid username or password");
            }
            else if(result === false)
            {

            }
            else
            {
                localStorage.setItem('token', result.token);
                localStorage.setItem('username', username);
                const token = jwtDecode(result.token);  
                const role = token["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];

                if(role === "admin") navigate('/adminDashboard');
                else if(role === "driver") navigate('/driverDashboard');
                else if(role === "regularUser") navigate('/regularUserDashboard');
            
            }
        }
    }

    const inputChanged = (set, setErr) => (e) =>
        {
            const value = e.target.value;
            set(value);
            setErr(value.trim() === "");
        }

    return (
        <div className='App'>
            <div className='LogIn'>
                <h1>Log In</h1>
                <form method='POST' onSubmit={clickLogin}>
                    <label className='label'>Username:&nbsp;</label>
                    <input type='text' onChange={inputChanged(setUsername, setUsernameErr)} placeholder='Type username...'></input>
                    <br />
                    <br />
                    <label className='label'>Password:&nbsp;</label>
                    <input type='password' onChange={inputChanged(setPassword, setPasswordErr)} placeholder='Type password...'></input>
                    <br />
                    <br />
                    <button className='button' type='submit' value='LogIn'>LogIn</button>
                </form>
                <p className='p'>Don't have an account?&nbsp;</p>
                <Link className='link' to="/register">Try to register.</Link>
            </div>
        </div>
    );

}


export default Login;