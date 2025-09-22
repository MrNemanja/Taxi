import axios from 'axios';

let serverEndpoint = process.env.REACT_APP_USER_REGISTER_URL

export default async function CallServerRegister(username, usernameErr, email, emailErr, password, passwordErr, confirmPassword, 
    confirmPasswordErr, name, nameErr, surname, surnameErr, address, addressErr ,dateOfBirth, dateOfBirthErr, role, imageUrl, 
    imageUrlErr, 
    ) {

        if(usernameErr)
        {
            alert("Username is required!");
            return false;
        }
        else if(emailErr)
        {
            alert("Email is not valid!");
            return false;
        }
        else if(passwordErr)
        {
            alert("Password is required!");
            return false;
        }
        else if(confirmPasswordErr)
        {
            alert("Passwords do not match!");
            return false;
        }
        else if(nameErr)
        {
            alert("Name is required!");
            return false;
        }
        else if(surnameErr)
        {
            alert("Surname is required!");
            return false;
        }
        else if(addressErr)
        {
            alert("Adress is required!");
            return false;
        }
        else if(dateOfBirthErr)
        {
            alert("Date of birth is not valid!");
            return false;
        }
        else if(imageUrlErr)
        {
            alert("Image is required!");
            return false;
        }
        else
        {
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
}