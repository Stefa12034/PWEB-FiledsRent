import { memo, PropsWithChildren } from "react";
import "./mainContent.scss";

export const MainContent = memo(({ children }: PropsWithChildren) => {
  return <main>
    {children}
  </main>;
});
