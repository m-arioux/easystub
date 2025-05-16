const express = require("express");
const { createProxyMiddleware } = require("http-proxy-middleware");
const morgan = require("morgan");
const app = express();
const port = 3000;

let fallback = { type: "NOT_FOUND" };

/**
 * @type express.Router | undefined
 */
let router = undefined;

/**
 * @typedef Endpoint
 * @prop {string} path
 * @prop {string} method
 * @prop {any} body
 * @prop {number} statusCode
 */

/**
 * @typedef PatchEndpoint
 * @prop {"EDIT" | "DELETE"} action
 * @prop {Endpoint} endpoint
 */

/**
 *
 * @param {Endpoint[]} endpoints
 */
function setupRouter(endpoints) {
  router = express.Router();

  router.use(morgan("combined"));

  router.use(express.json());

  router.get("/healthz", (req, res) => {
    res.send();
  });

  router.get("/_admin/endpoints", (req, res) => {
    res.send(endpoints);
  });

  router.post("/_admin/endpoints", (req, res) => {
    /**
     * @type {PatchEndpoint}
     */
    const body = req.body;
    const endpoint = body.endpoint;

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

  endpoints.forEach((endpoint) => {
    router.use(endpoint.path, (req, res, next) => {
      if (req.method !== endpoint.method) {
        next();
        return;
      }

      res.status(endpoint.statusCode);
      res.send(endpoint.body);
    });
  });

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
  router(req, res, next);
});

setupRouter([]);

app.listen(port, () => {
  console.log(`Server listening on port ${port}`);
});
