import axios from "axios"

let loginEndpoint = process.env.REACT_APP_USER_LOGIN_URL

export default async function CallServerLogin(username, usernameErr, password, passwordErr)
{
    if(usernameErr)
        {
            alert("Username is required!");
            return false;
        }
    else if(passwordErr)
        {
            alert("Password is required!");
            return false;
        }
    else
    {
        const dataLogin = new FormData();
        dataLogin.append("username", username);
        dataLogin.append("password", password);

        try
            {
              const response = await axios.post(loginEndpoint, dataLogin, {
                    headers: {
                        'Content-Type' : 'multipart/form-data'
                    }
                });
                 
                return response.data;

            }catch(error)
            {     
                return error.response.data.err[0];
            }
    }

}