import express from "express";
import { createProxyMiddleware } from "http-proxy-middleware";
import morgan from "morgan";
import cors from "cors";
import { Endpoint } from "./Endpoint.model";

const app = express();
const port = 3000;

type Fallback = {
  type: "NOT_FOUND" | "REDIRECT" | "JSON";
  statusCode: number;
  json?: any;
  baseUrl?: string;
};

let fallback: Fallback = { type: "NOT_FOUND", statusCode: 404 };

let router: express.Router | undefined = undefined;

type PatchEndpoint = {
  action: "EDIT" | "DELETE";
  endpoint: Endpoint;
};

function setupRouter(endpoints: Endpoint[]) {
  router = express.Router();

  router.use(morgan("combined"));

  router.use(cors());

  router.use(express.json());

  router.get("/healthz", (req, res) => {
    res.send();
  });

  router.get("/_admin/endpoints", (req, res) => {
    res.send(endpoints);
  });

  router.post("/_admin/endpoints", (req, res) => {
    const endpoint: Endpoint = req.body;

    const newEndpoints = [...endpoints, endpoint];
    setupRouter(newEndpoints);

    res.status(200);
    res.send(newEndpoints);
  });

  router.get("/_admin/fallback", (req, res) => {
    res.send(fallback);
  });

  router.patch("/_admin/fallback", (req, res) => {
    console.log("received", req.body);
    const newFallback = req.body; // TODO: validate

    fallback = newFallback;

    res.send();
  });

  for (const endpoint of endpoints) {
    router.use(endpoint.path, (req, res, next) => {
      if (req.method !== endpoint.method) {
        next();
        return;
      }

      res.status(endpoint.statusCode);
      res.send(endpoint.body);
    });
  }

  router.use((req, res, next) => {
    if (fallback.type === "JSON") {
      res.status(fallback.statusCode);
      res.send(fallback.json);
      next();
      return;
    }

    if (fallback.type === "REDIRECT") {
      createProxyMiddleware({ target: fallback.baseUrl, changeOrigin: true })(
        req,
        res,
        next
      );
      return;
    }

    res.status(404);
    res.send();
    next();
  });
}

app.use((req, res, next) => {
  if (!router) throw new Error("router has not been initialized");

  router(req, res, next);
});

setupRouter([]);

app.listen(port, () => {
  console.log(`Server listening on port ${port}`);
});
