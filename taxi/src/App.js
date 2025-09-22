import { BrowserRouter, Route, Routes } from "react-router-dom";
import Register from "./components/Register/Register";
import Login from "./components/Login/Login";
import AdminDashboard from './components/AdminDashboard/AdminDashboard';
import DriverDashboard from './components/DriverDashboard/DriverDashboard';
import RegularUserDashboard from './components/RegularUserDashboard/RegularUserDashboard';
import './App.css';
import GoogleRegister from "./components/Register/GoogleRegister";


function App() {
  return (
    <div>
      <BrowserRouter>
        <Routes>
          <Route path='/' element={<Login/>}/>
          <Route path='/register' element={<Register/>}/>
          <Route path='/googleRegister' element={<GoogleRegister />}/>
          <Route path='/adminDashboard' element={<AdminDashboard />}/>
          <Route path='/driverDashboard' element={<DriverDashboard />}/>
          <Route path='/regularUserDashboard' element={<RegularUserDashboard />}/>
        </Routes>
      
      </BrowserRouter>
        
    </div>
   
  );
}

export default App;
