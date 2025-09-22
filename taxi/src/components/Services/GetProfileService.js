import axios from "axios";

let endpoint = process.env.REACT_APP_USER_PROFILE_URL
let endpointUpdate = process.env.REACT_APP_USER_UPDATE_URL
let endpointIsGoogle = process.env.REACT_APP_USER_ISGOOGLE_URL


function ValidateDate(birthday)
{
    const today = new Date();
    const tYear = today.getFullYear();
    const tMonth = today.getMonth() + 1;
    const tDay = today.getDate();

    const values = birthday.split('-');
    const year = parseInt(values[0]);
    const month = parseInt(values[1]);
    const day = parseInt(values[2]);
        

    if(year > tYear) return false;
    else if(year === tYear)
    {
        if(month > tMonth) return false;
        else if(month === tMonth)
        {
            if(day > tDay) return false;
            else return true;
        }
            else return true;
    } 
        else return true;  

}

function ValidateEmail(email)
{
    const valid = /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
    if(valid) return true;
    else return false;

}

export async function CallProfileUpdate(username, Email, Name, Surname, Address, Birthday, Image) {

        const isValidEmail = ValidateEmail(Email);
        const isValidDate = ValidateDate(Birthday);
        
        if(!isValidEmail)
        {
            alert("Email is not valid!");
            return false;
        }
        else if(!isValidDate)
        {
            alert("Birthday is not valid!");
            return false;
        }
        else
        {
            const data = new FormData();
            data.append("username", username);
            data.append("email", Email);
            data.append("name", Name);
            data.append("surname", Surname);
            data.append("address", Address);
            data.append("birthday", Birthday);
            data.append("imageUrl", Image); 

            try
            {
              const response = await axios.post(endpointUpdate, data, {
                    headers: {
                        'Content-Type' : 'multipart/form-data'
                    }
                    
            });
                
                return response.data;

            }catch(error)
            {     
                alert(error.response.data.err[0])
                return error.response.data.err[0];
            }
        }
}

export default async function CallGetProfile(username)
{
        const data = new FormData();
        data.append("username", username);

        try
            {
              const response = await axios.post(endpoint, data, {
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

export async function CallIsGoogle(username) {
    
        const data = new FormData();
        data.append("username", username);

        try
            {
              const response = await axios.post(endpointIsGoogle, data, {
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
