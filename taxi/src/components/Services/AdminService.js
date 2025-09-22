import axios from "axios";

let endpointPending = process.env.REACT_APP_USER_PENDING_URL
let endpointReject = process.env.REACT_APP_USER_REJECT_URL
let endpointAccept = process.env.REACT_APP_USER_ACCEPT_URL
let endpointAllRides = process.env.REACT_APP_DRIVE_ALLRIDES_URL
let endpointBlock = process.env.REACT_APP_DRIVER_BLOCK_URL
let endpointUnblock = process.env.REACT_APP_DRIVER_UNBLOCK_URL

export default async function CallGetPendingDrivers(token)
{
        try
            {
              const response = await axios.get(endpointPending, {
                    headers: {
                        Authorization : `Bearer ${token}`,
                        //'Content-Type' : 'multipart/form-data'
                        
                    }
                });
                
                return response.data;

            }catch(error)
            {     
                return error.response.data;
            }
}

export async function CallRejectVerification(token, username)
{
    const data = new FormData();
    data.append("username", username);
    
    try
    {
        await axios.post(endpointReject, data, {
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

export async function CallAcceptVerification(token, username) {
    
    const data = new FormData();
    data.append("username", username);
    
    try
    {
        await axios.post(endpointAccept, data, {
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

export async function CallGetAllRides(token) {
    
    try
    {
       const response = await axios.get(endpointAllRides, {
            headers: {
                Authorization : `Bearer ${token}`,
                //'Content-Type' : 'multipart/form-data'
            }
        });
        
        return response.data;

    }catch(error)
    {     
        return error.response.data;
    }
}

export async function CallBlockDriver(token, username) {
    
    const data = new FormData();
    data.append("username", username);

    try
    {
       const response = await axios.post(endpointBlock, data, {
            headers: {
                Authorization : `Bearer ${token}`,
                'Content-Type' : 'multipart/form-data'
            }
        });
        
        return response.data;

    }catch(error)
    {     
        return error.response.data;
    }
}

export async function CallUnblockDriver(token, username) {
    
    const data = new FormData();
    data.append("username", username);

    try
    {
       const response = await axios.post(endpointUnblock, data, {
            headers: {
                Authorization : `Bearer ${token}`,
                'Content-Type' : 'multipart/form-data'
            }
        });
        
        return response.data;

    }catch(error)
    {     
        return error.response.data;
    }
}

