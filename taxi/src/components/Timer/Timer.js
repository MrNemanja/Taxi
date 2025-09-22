import React, {useState, useEffect} from "react";
import './Timer.css'
import CallSetRating from "../Services/TimeService";

function Timer({time, id, type, driver})
{
    const token = localStorage.getItem('token');
    const initialTime = time;
    const [timeRemaining, setTimeRemaining] = useState(initialTime);
  
    const setRating = async () =>
    {
      await CallSetRating(id, token);
    }

    useEffect(() => {
      const timerInterval = setInterval(() => {
        setTimeRemaining((prevTime) => {
          if (prevTime === 0) {
            clearInterval(timerInterval);
            return 0;
          } else {
            return prevTime - 1;
          }
        });
      }, 1000);
  
      return () => clearInterval(timerInterval);
    }, []); 

    useEffect(() => {
      
      (async () => {

        if(timeRemaining === 0 && type === false)
          {
            await setRating();
            window.location.reload();
            
          }
      })();

    }, [timeRemaining]);
  
    const hours = Math.floor(timeRemaining / 3600);
    const minutes = Math.floor((timeRemaining % 3600) / 60);
    const seconds = timeRemaining % 60;
  
    return (
      <>
      <div className="backgr">
        {type === true && driver === false ? (
            <label className="label1">Your driver is here in:</label>
        ) : type === true && driver === true ? (
          <label className="label1">You will pick your customer in:</label>
      ) : type === false ? (
          <label className="label1">The ride will end in:</label>
        ) : null}
        &nbsp;
        <label className="label2">{`${hours}h ${minutes}m ${seconds}s`}</label>
      </div>
      </>
    );
}

export default Timer;