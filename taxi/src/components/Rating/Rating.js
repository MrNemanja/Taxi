import "./Rating.css";
import StarRate from "./StarRate";

function Rating({guid}) {

    return(
    <div className="main">
        <h1 className="h1">Give driver a rating:</h1>
            <StarRate id={guid} />
    </div>
    );
}

export default Rating;