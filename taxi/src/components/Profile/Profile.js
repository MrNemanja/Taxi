import { useEffect, useState, useLayoutEffect } from "react";
import "./Profile.css";
import CallGetProfile from "../Services/GetProfileService";
import { CallProfileUpdate } from "../Services/GetProfileService";
import { CallIsGoogle } from "../Services/GetProfileService";
import { jwtDecode } from "jwt-decode";
import { useNavigate } from "react-router-dom";

function Profile() {

    const [email, setEmail] = useState("");
    const [name, setName] = useState("");
    const [surname, setSurname] = useState("");
    const [address, setAddress] = useState("");
    const [birthday, setBirthday] = useState("");
    const [role, setRole] = useState("");
    const [verification, setVerification] = useState("");
    const [imageUrl, setImageUrl] = useState("");
    const username = localStorage.getItem('username');
    const token = localStorage.getItem('token');
    const navigate = useNavigate();
    const [google, setGoogle] = useState(false);
    let endpoint = process.env.REACT_APP_USER_IMGPROFILE_URL

    useLayoutEffect(() =>
        {
            if(token === null)
            {
                navigate("/");
            }
            else
            {

                const decoded = jwtDecode(token);
                let currentDate = new Date();
                if(decoded.exp * 1000 < currentDate.getTime())
                {
                    localStorage.clear();
                    navigate("/");
                }

                IsGoogle();
            }
        
        }, []);

    const IsGoogle = async () =>
    {
        const result = await CallIsGoogle(username);

        if(result === true) setGoogle(true);
        else if(result === false) setGoogle(false);
    }

    const getProfile = async () =>
    {
        const result = await CallGetProfile(username);
        setEmail(result.email);
        setName(result.name);
        setSurname(result.surname);
        setAddress(result.address);
        
        const date = new Date(result.birthday);
        const month = date.getMonth() + 1;
        setBirthday(date.getDate() + '/' + month + '/' + date.getFullYear());

        if(result.role === 0) setRole("Admin");
        else if (result.role === 1) setRole("Driver");
        else if (result.role === 2) setRole("RegularUser");

        if(result.verified === 0) setVerification("Accepted");
        else if (result.verified === 1) setVerification("Rejected");
        else if (result.verified === 2) setVerification("Pending");

        if(google === false)
        {
            const link = endpoint + "?fileName=" + result.imageUrl;
            setImageUrl(link);
        }
        else
        {
           setImageUrl(result.imageUrl);
        }
      
    }

    const ClickUpdate = async (e) =>
    {
        e.preventDefault();

        const formEl = document.forms.update;
        const formData = new FormData(formEl);


        var Email = formData.get('email');
        var Name = formData.get('name');
        var Surname = formData.get('surname');
        var Address = formData.get('address');
        var Birthday = formData.get('birthday');
        var Image = formData.get('image');

        if(Email === "") Email = email;
        if(Birthday === "") Birthday = birthday;
        if(Name === "") Name = name;
        if(Surname === "") Surname = surname;
        if(Address === "") Address = address;
        if(Image === "") Image = imageUrl;


        const result = await CallProfileUpdate(username, Email, Name, Surname, Address, Birthday, Image);
        
        alert("Your update is successfull!");
        window.location.reload();


    }

    useEffect(() => 
        {
            if(token !== null) getProfile();
    
        }, [google]);

    return(

        <div className="backgroundP">

            <div className="divShow">
                <h2 className="h2">Your Profile</h2>
                <div className="divData">
                    <b><i><label className="labelP1">Email:&nbsp;</label></i></b>
                    <b><i><label className="labelP">{email}</label></i></b>
                    <br/>
                    <br/>
                    <b><i><label className="labelP1">Name:&nbsp;</label></i></b>
                    <b><i><label className="labelP">{name}</label></i></b>
                    <br/>
                    <br/>
                    <b><i><label className="labelP1">Surname:&nbsp;</label></i></b>
                    <b><i><label className="labelP">{surname}</label></i></b>
                    <br/>
                    <br/>
                    <b><i><label className="labelP1">Address:&nbsp;</label></i></b>
                    <b><i><label className="labelP">{address}</label></i></b>
                    <br/>
                    <br/>
                    <b><i><label className="labelP1">Birthday:&nbsp;</label></i></b>
                    <b><i><label className="labelP">{birthday}</label></i></b>
                    <br/>
                    <br/>
                    <b><i><label className="labelP1">Role:&nbsp;</label></i></b>
                    <b><i><label className="labelP">{role}</label></i></b>
                    <br/>
                    <br/>
                    {role === "Driver" ? ( 
                        <>
                        <b><i><label className="labelP1">Verification request:&nbsp;</label></i></b>
                        <b><i><label className="labelP">{verification}</label></i></b>
                        </>
                     ) : null}
                    
                </div>
                <div className="divImage"><img src={imageUrl} width="100%" height="100%" referrerPolicy="no-referrer" /></div>
            </div>
            <div className="divUpdate">
                <h2 className="h2">Update Profile</h2>
                <div className="divForm">
                <form method="POST" onSubmit={ClickUpdate} id="update">
                <label className='label'>Email: &nbsp;</label>
                    <input type='text' placeholder="Type email..." name="email"></input>
                    <br />
                    <br />
                    <label className='label'>Name: &nbsp;</label>
                    <input type='text' placeholder="Type name..." name="name"></input>
                    <br />
                    <br />
                    <label className='label'>Surname: &nbsp;</label>
                    <input type='text' placeholder="Type surname..." name="surname"></input>
                    <br />
                    <br />
                    <label className='label'>Address: &nbsp;</label>
                    <input type='text' placeholder="Type address..." name="address"></input>
                    <br />
                    <br />
                    <label className='label'>Date of birth: &nbsp;</label>
                    <input type="date" name="birthday"></input>
                    <br />
                    <br />
                    <label className='label'>Image: &nbsp;</label>
                    <input type="file" name="image"></input>
                    <br />
                    <br />
                    <button className='button' type='submit' value='Update'>Update</button>
                </form>
                </div>
            </div>
        </div>

    );


}

export default Profile;