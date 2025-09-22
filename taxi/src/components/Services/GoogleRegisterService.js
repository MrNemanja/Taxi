import axios from 'axios';

let serverEndpoint = process.env.REACT_APP_USER_REGISTERGOOGLE_URL

export default async function CallGoogleRegister(username, password, passwordErr, confirmPassword, confirmPasswordErr, email, name, surname, address, addressErr, dateOfBirth, dateOfBirthErr, role, imageUrl) {
        
        if(passwordErr)
        {
            alert("Password is required!");
            return "err";
        } 
        if(confirmPasswordErr)
        {
                alert("Passwords do not match!");
                return "err";
        }
        if(addressErr)
        {
            alert("Adress is required!");
            return "err";
        }
        else if(dateOfBirthErr)
        {
            alert("Date of birth is not valid!");
            return "err";
        }

        const data = new FormData();
        data.append("username", username);
        data.append("password", password);
        data.append("email", email);
        data.append("name", name);
        data.append("surname", surname);
        data.append("birthday", dateOfBirth);
        data.append("address", address);
        data.append("role", role);
        data.append("imageUrl", imageUrl); 

        try
        {
            await axios.post(serverEndpoint, data, {
                headers: {
                    'Content-Type' : 'multipart/form-data'
                }
            });
                 
            return true;

        }catch(error)
        {     
            alert(error.response.data.err[0])
            return false;
        }
}
