import axios from "axios";

let endpointR = process.env.REACT_APP_USER_REQUEST_URL
let endpointG = process.env.REACT_APP_DRIVE_GET_URL
let endpointA = process.env.REACT_APP_DRIVE_ACCEPT_URL
let endpointBlockD = process.env.REACT_APP_DRIVE_BLOCKD_URL
let endpointMyRides = process.env.REACT_APP_DRIVE_MYRIDES_URL
let endpointisBlocked = process.env.REACT_APP_DRIVER_ISBLOCK_URL

export default async function CallGetUserRequest(username, token)
{
        const data = new FormData();
        data.append("username", username);

        try
            {
              const response = await axios.post(endpointR, data, {
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

export async function CallGetRides(token, username) 
{

    const data = new FormData();
    data.append("username", username);

        try
            {
              const response = await axios.post(endpointG, data ,{
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

export async function AcceptRide(id, username, token) 
{
    
    const data = new FormData();
    data.append("id", id);
    data.append("username", username);

    try
        {
          const response = await axios.post(endpointA, data, {
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

export async function CallRideD(token, username) {
    
    const data = new FormData();
    data.append("username", username);

    try
        {
            const response = await axios.post(endpointBlockD, data, {
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

export async function CallGetMyRides(token, username) {
    
    const data = new FormData();
    data.append("username", username);

    try
        {
            const response = await axios.post(endpointMyRides, data, {
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

export async function CallIsBlocked(token, username) {
    
    const data = new FormData();
    data.append("username", username);

    try
        {
            const response = await axios.post(endpointisBlocked, data, {
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