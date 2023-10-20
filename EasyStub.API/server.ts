import cors from "cors";
import express from "express";
import asyncHandler from "express-async-handler";
import morgan from "morgan";
import { UnsavedEndpoint } from "./Endpoint.model";
import { setupFallback, useFallback } from "./fallback";
import { EndpointsRepository } from "./infrastructure/endpoints.repository";

const app = express();
const port = 3000;

let router: express.Router | undefined = undefined;

function setupRouter(endpointsRepository: EndpointsRepository) {
  router = express.Router();

  router.use(morgan("combined"));

  router.use(cors());

  router.use(express.json());

  router.get("/healthz", (req, res) => {
    res.send();
  });

  router.get("/_admin/endpoints", (req, res) => {
    res.send(endpointsRepository.getAll());
  });

  router.post(
    "/_admin/endpoints",
    asyncHandler(async (req, res) => {
      const endpoint: UnsavedEndpoint = req.body;

      const createdEndpoint = await endpointsRepository.create(endpoint);

      setupRouter(endpointsRepository);

      res.status(200);
      res.send(createdEndpoint);
    })
  );

  setupFallback(router);

  for (const endpoint of endpointsRepository.getAll()) {
    router.use(endpoint.path, (req, res, next) => {
      if (req.method !== endpoint.method) {
        next();
        return;
      }

      res.status(endpoint.statusCode);
      res.send(endpoint.body);
    });
  }

  useFallback(router);
}

app.use((req, res, next) => {
  if (!router) throw new Error("router has not been initialized");

  router(req, res, next);
});

setupRouter(new EndpointsRepository());

app.listen(port, () => {
  console.log(`Server listening on port ${port}`);
});
