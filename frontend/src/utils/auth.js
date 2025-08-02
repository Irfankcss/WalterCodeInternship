import jwtDecode from "jwt-decode";
function isAdmin() {
  const token = localStorage.getItem("token");
  if (!token) return false;

  const decodedToken = jwtDecode(token);
  return decodedToken.role === "Admin";
}
export default isAdmin
