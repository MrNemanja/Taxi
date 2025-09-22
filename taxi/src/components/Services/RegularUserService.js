import axios from "axios";

let endpointOrder = process.env.REACT_APP_DRIVE_ORDER_URL;
let endpointIsOrder = process.env.REACT_APP_DRIVE_ISORDER_URL;
let endpointIsAccept = process.env.REACT_APP_DRIVE_ISACCEPT_URL;
let endpointDelete = process.env.REACT_APP_DRIVE_DELETE_URL;
let endpointConfirm = process.env.REACT_APP_DRIVE_CONFIRM_URL;
let endpointBlock = process.env.REACT_APP_DRIVE_BLOCKED_URL;
let endpointPrevious = process.env.REACT_APP_DRIVE_PREVIOUS_URL;
let endpointIsRate = process.env.REACT_APP_DRIVE_ISRATING_URL;

export default async function CallOrderRide(startAddress, startAddressErr, endAddress, endAddressErr, token, username)
{
    if(startAddressErr)
        {
            alert("Start address is required!");
            return false;
        }
    else if(endAddressErr)
        {
            alert("End address is required!");
            return false;
        }
    else
    {
        const data = new FormData();
        data.append("startAddress", startAddress);
        data.append("endAddress", endAddress);
        data.append("username", username);

        try
            {
                await axios.post(endpointOrder, data, {
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

export async function CallIsOrdered(token, username) {
    
        const data = new FormData();
        data.append("username", username);

        try
            {
                const response = await axios.post(endpointIsOrder, data, {
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

export async function CallIsAccepted(token, username) {
    
    const data = new FormData();
    data.append("username", username);

    try
        {
            const response = await axios.post(endpointIsAccept, data, {
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

export async function CallDeleteRide(token, username) {
    
    const data = new FormData();
    data.append("username", username);

    try
        {
            const response = await axios.post(endpointDelete, data, {
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

export async function CallConfirm(token, username) {
    
    const data = new FormData();
    data.append("username", username);

    try
        {
           const response = await axios.post(endpointConfirm, data, {
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

export async function CallGetPreviousRides(token, username) {
    
    const data = new FormData();
    data.append("username", username);

    try
        {
            const response = await axios.post(endpointPrevious, data, {
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

export async function CallIsRating(token, username) {
    
    const data = new FormData();
    data.append("username", username);

    try
        {
            const response = await axios.post(endpointIsRate, data, {
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
