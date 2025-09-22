import './RegularUser.css';
import { useState, useMemo, useLayoutEffect } from 'react';
import CallOrderRide from '../Services/RegularUserService';
import { CallIsOrdered } from '../Services/RegularUserService';
import { CallIsAccepted } from '../Services/RegularUserService';
import { CallDeleteRide } from '../Services/RegularUserService';
import { CallConfirm } from '../Services/RegularUserService';
import { jwtDecode } from 'jwt-decode';
import { useNavigate } from 'react-router-dom';

function NewRide() {

    const token = localStorage.getItem('token');
    const username = localStorage.getItem('username');
    const navigate = useNavigate();
    const [startAddress, setStartAddress] = useState('');
    const [startAddressErr, setStartAddressErr] = useState(true);
    const [endAddress, setEndAddress] = useState('');
    const [endAddressErr, setEndAddressErr] = useState(true);
    var order = false;
    const [price, setPrice] = useState(0);
    const [time, setTime] = useState(0);

    useLayoutEffect(() =>
        {
            const decoded = jwtDecode(token);
            let currentDate = new Date();
            if(decoded.exp * 1000 < currentDate.getTime())
            {
                localStorage.clear();
                navigate("/");
            }

        }, []); 

    const isAccepted = async () =>
    {
      const result = await CallIsAccepted(token, username)
      
      setTime(result.item2);
      setPrice(result.item1);
      
    }

    const isOrdered = async () =>
    {
        const result = await CallIsOrdered(token, username);

        if(result === true) 
        {
            return true;
        }
        
        else if(result === false) return false;
    }

    const inputChanged = (set, setErr) => (e) =>
        {
            const value = e.target.value;
            set(value);
            setErr(value.trim() === "");
        }

    const clickOrder = async (e) =>
    {
        e.preventDefault();

        order =  await isOrdered();

        console.log(order);

        if(order === true) alert("Wait for the driver's approval.");

        else if(order === false)
        {
            const result = await CallOrderRide(startAddress, startAddressErr, endAddress, endAddressErr, token, username);

            if(result === true)
            {
                alert("Your ride has been successfully ordered, wait for the driver's approval.");
            }
        }
    }

    const renderPage = () =>
    {   console.log(price);
        return (
            <>
            { price === 0 ? (
                <div className='Order'>
                <h1 className='h1'>Order new ride</h1>
                <form method='POST' onSubmit={clickOrder}>
                    <b><label>Start Address:&nbsp;</label></b>
                    <input type='text' onChange={inputChanged(setStartAddress, setStartAddressErr)} placeholder='Type start address...'></input>
                    <br />
                    <br />
                    <b><label>End Address:&nbsp;</label></b>
                    <input type='text' onChange={inputChanged(setEndAddress, setEndAddressErr)} placeholder='Type end address...'></input>
                    <br />
                    <br />
                    <button className='button' type='submit' value='Order'>Order</button>
                </form>
                </div>
                ): price > 0 ? (
                <div className='Show'>
                    <h1 className='h11'>Ride Information</h1>
                        <b><i><label className='labelS'>Ride price: {price} RSD&nbsp;</label></i></b>
                        <br />
                        <br />
                        <b><i><label className='labelS'>Your driver is here in {time} minutes.&nbsp;</label></i></b>
                        <br />
                        <br />
                        <button className='button' type='submit' value='Confirm' onClick={clickConfirm}>Confirm</button>
                        &nbsp;&nbsp;
                        <button className='button' value='Cancel' onClick={clickCancel}>Cancel</button>
                        
                </div>
            ) : null}
            </>
        );
    }

    const ret = useMemo(() =>
        {
          isAccepted();

        }, []); 

    const clickConfirm = async () =>
    {
        const result = await CallConfirm(token, username);

        if(result === true)
        {
            alert("Your driver is on the way! Please wait.");
            window.location.reload();
        }
        else if(result === false) alert("Something is wrong.");
    }

    const clickCancel = async () =>
    {
        const result = await CallDeleteRide(token, username)

        if(result === true)
        { 
            alert("You refused the ride.");
            window.location.reload();
        }

        else if(result === false) alert("Something is wrong.");
    }
 
   return(
            <div className='backgroundR'>
                { renderPage() }
            </div>
   );

}
export default NewRide;