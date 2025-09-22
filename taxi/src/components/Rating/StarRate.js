import "./Rating.css";
import { useState } from "react";
import { FaStar } from "react-icons/fa"
import CallRateDriver from "../Services/RatingService";
import Rating from "./Rating";


function StarRate({id})
{
    const [rating, setRating] = useState(0);
    const [rateColor, setRateColor] = useState(null);
    const token = localStorage.getItem("token");

    const AddRating = async (e) =>
    {
        e.preventDefault();

        const result = await CallRateDriver(id, rating, token);

        if(result === true)
        {
            alert("Your rating has been successfully added.");
            window.location.reload();
        }

    }

    return (
        <>
        <form method="POST" onSubmit={AddRating}>
        {[...Array(5)].map((star, index) =>
        {
            const currentRate = index + 1;
            return (
                <>
                
                <label>
                    <input type="radio" name="rate" value={currentRate} onClick={() => setRating(currentRate)}/>
                    <FaStar size={50} color={currentRate <= (rateColor || rating) ? "red" : "grey"}/>
                </label>
                
                </>
            );
        })}
        <br/>
        <br/>
        <button className="add" onSubmit={AddRating} type="Submit" value="Add">Add</button>
        </form>
        </>
    );
}
export default StarRate;