import axios from 'axios'

let endpoint = process.env.REACT_APP_DRIVER_ADDRATING_URL;

export default async function CallRateDriver(id, rate, token)
{
    if(rate === 0)
    {
        alert("You must rate your driver.");
        return false;
    }
    else
    {

    console.log(id);
    const data = new FormData();
    data.append("id", id);
    data.append("rate", rate);

    try
        {
            await axios.post(endpoint, data, {
                headers: {
                    Authorization : `Bearer ${token}`,
                    'Content-Type' : 'multipart/form-data'
                }
            });
             
            return true;

        }catch(error)
        {     
            return false;
        }
    }
}