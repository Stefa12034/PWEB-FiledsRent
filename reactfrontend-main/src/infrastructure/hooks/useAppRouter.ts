import { useCallback } from "react";
import { useNavigate } from "react-router-dom";
import { AppRoute } from "routes";

export const useAppRouter = () => {
  const navigate = useNavigate();

  const redirectToHome = useCallback(
    () => navigate(AppRoute.Index),
    [navigate]
  );

  const redirectToUsers = useCallback(
    () =>
      navigate({
        pathname: AppRoute.Users
      }),
    [navigate]
  );

  const redirectToUsersFiles = useCallback(
    () =>
      navigate({
        pathname: AppRoute.Users
      }),
    [navigate]
  );

  return {
    redirectToHome,
    redirectToUsers,
    redirectToUsersFiles,
    navigate
  };
};
