import { Grid } from "@mui/material";
import { Container } from "@mui/system";
import { FC } from "react";
import './footer.scss';

export const Footer: FC<{}> = () => {
  const year = new Date().getFullYear();

  return <div className="website__footer">
    <Grid container item direction="row" xs={12}>
      <Grid container item direction="column" xs={12}>
        <Container>
          <div className="app__copyright">
            &copy; {year} • All rights reserved
          </div>
        </Container>
      </Grid>
    </Grid>
  </div>
};
