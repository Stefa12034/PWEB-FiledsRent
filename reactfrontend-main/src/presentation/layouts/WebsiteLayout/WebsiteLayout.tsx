import { Footer } from "../Footer";
import { MainContent } from "../MainContent";
import { Navbar } from "../Navbar";
import { Fragment, memo, PropsWithChildren } from "react";

export const WebsiteLayout = memo(
  (props: PropsWithChildren<{}>) => {
    const { children } = props;

    return <Fragment>
      <Navbar />
      <MainContent>{children}</MainContent>
      <Footer />
    </Fragment>
  }
);
