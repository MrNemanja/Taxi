import axios from "axios";

let endpointRating = process.env.REACT_APP_DRIVE_SETRATING_URL;


export default async function CallSetRating(id, token)
{
        const data = new FormData();
        data.append("id", id);

        try
            {
                await axios.post(endpointRating, data, {
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