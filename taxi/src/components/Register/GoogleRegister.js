import "./Register.css";
import { useState } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import CallGoogleRegister from "../Services/GoogleRegisterService";


function GoogleRegister() {

    const [password, setPassword] = useState('');
    const [passworderr, setPasswordErr] = useState(true);
    const [confirmPassword, setConfirmPassword] = useState('');
    const [confirmPasswordErr, setConfirmPasswordErr] = useState(true);
    const [address, setAddress] = useState('');
    const [addressErr, setAddressErr] = useState(true);
    const [dateOfBirth, setDateOfBirth] = useState('');
    const [dateOfBirthErr, setDateOfBirthErr] = useState(true);
    const [role, setRole] = useState("RegularUser");

    const navigate = useNavigate();
    const location = useLocation();

    const clickGoogleRegister = async (e) =>
    {
        e.preventDefault();
        
        const username = location.state.username;
        const email = location.state.email;
        const name = location.state.name;
        const surname = location.state.surname;
        const imageUrl = location.state.imageUrl;

        const result = await CallGoogleRegister(username, password, passworderr, confirmPassword, confirmPasswordErr, email, name, surname, address, addressErr, dateOfBirth, dateOfBirthErr, role, imageUrl);
        
        if(result === true)
        {
            alert("Your registration is successfull");
            navigate("/");
        }
            
    }

    const confirmPasswordChanged = (e) => {
        const value = e.target.value;
        setConfirmPassword(value);
        setConfirmPasswordErr(value !== password);
    };
    
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

    const roleChanged = (e) => 
    {
        const value = e.target.value;
        setRole(value);
    }

    return(
        <div className="App">
            <div className='RegisterGoogle'>
                <h1>Register with Google</h1>
                <form onSubmit={clickGoogleRegister} method='POST'>
                    <label className='label'>Password: &nbsp;</label>
                    <input type='password' placeholder="Type address..." onChange={inputChanged(setPassword, setPasswordErr)} value={password || ''}></input>
                    <br />
                    <br />
                    <label className='label'>Confirm Password: &nbsp;</label>
                    <input type='password' placeholder="Type address..." onChange={confirmPasswordChanged} value={confirmPassword || ''}></input>
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
                    <button className='button' type='submit' value='Register'>Register</button> 

                </form>
            </div>
        </div>

    );

}

export default GoogleRegister;